﻿using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Proxy.RabbitMQ;

public sealed class CallSession : IDisposable
{
    private volatile IModel? _subChannel;
    private readonly IConnection _subConnection;
    private readonly BasicDeliverEventArgs _e;
    private readonly ILogger _logger;
    private readonly SubWatcher _subWatcher;
    private readonly string _serviceToClientQueue;
    private volatile bool _disposed;
    private readonly bool _isPost;
    private volatile string? _consumerTag;
    private readonly AsyncLock _lock_Receive = new();
    public event AsyncEventHandler<EventArgsT<ReadOnlyMemory<byte>>>? ReceivedAsync;
    public event EventHandler? Disconnected;

    public CallSession(IConnection subConnection, SubWatcher subWatcher, BasicDeliverEventArgs e, ILogger logger)
    {
        _isPost = e.BasicProperties.ReplyTo == null;
        _serviceToClientQueue = e.BasicProperties.ReplyTo!;
        _subConnection = subConnection;
        _e = e;
        _logger = logger;
        _subConnection.ConnectionShutdown += ConnectionShutdown;
        _subWatcher = subWatcher;
        _subWatcher.Disconnected += SubWatcherDisconnected;
        _subWatcher.Add(_serviceToClientQueue);
    }

    private void ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _logger.LogWarning($"MainChannel shutdown, {e.ReplyCode}, {e.ReplyText}.");
        OnDisconnected();
        Dispose();
    }

    private void SubWatcherDisconnected(object? sender, EventArgsT<string> e)
    {
        if (e.Value != _serviceToClientQueue)
            return;

        OnDisconnected();
        Dispose();
    }

    public bool Start()
    {
        if (!_isPost)
        {
            if (!DeclareCallBack())
                return false;
        }

#pragma warning disable CS4014
        //on exception here
        OnReceivedAsync(new EventArgsT<ReadOnlyMemory<byte>>(_e.Body));
#pragma warning restore CS4014

        return true;
    }

    public void Send(ReadOnlyMemory<byte> buffer)
    {
        _subChannel.BasicPublish("", _serviceToClientQueue, null!, buffer);
    }

    private bool DeclareCallBack()
    {

        try
        {
            _subChannel = _subConnection.CreateModel();
            var clientToServiceQueue = _subChannel.QueueDeclare().QueueName;
            Debug.WriteLine($"service: _clientToServiceQueue: {clientToServiceQueue}");
            var clientToServiceConsumer = new AsyncEventingBasicConsumer(_subChannel);
            clientToServiceConsumer.Received += (_, e) => OnReceivedAsync(new EventArgsT<ReadOnlyMemory<byte>>(e.Body));
            _consumerTag = _subChannel.BasicConsume(clientToServiceQueue, true, clientToServiceConsumer);
            Send(Encoding.UTF8.GetBytes(clientToServiceQueue));
            return true;
        }
        catch
        {
            _logger.LogWarning("DeclareCallBack failed.");
            return false;
        }
    }

    private async Task OnReceivedAsync(EventArgsT<ReadOnlyMemory<byte>> e)
    {
        //Consumer will has 2 threads invoke simultaneously.
        //lock here make sure the msg sequence
        using (await _lock_Receive.LockAsync())
            await ReceivedAsync.InvokeAsync(this, e);
    }

    private void OnDisconnected()
    {
        Disconnected?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;

        _subConnection.ConnectionShutdown -= ConnectionShutdown;
        _subWatcher.Disconnected -= SubWatcherDisconnected;
        _subWatcher.Remove(_serviceToClientQueue);
        _subChannel?.TryBasicCancel(_consumerTag, _logger);
        _subChannel?.Close();
    }
}