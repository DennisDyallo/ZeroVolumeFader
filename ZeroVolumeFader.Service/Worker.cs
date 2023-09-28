using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ZeroVolumeFader.Service;

public class Worker : IHostedService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMediator _mediator;

    public Worker(ILogger<Worker> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker running");
        
        var fadeRequest = new FadeRequest
        {
            FadeOptions = new FadeOptions
            {
                WaitTime = TimeSpan.FromSeconds(1), 
                FadeTime = TimeSpan.FromSeconds(5) 
            }
        };

        var task = _mediator.Send(fadeRequest, cancellationToken);
        return task;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker shutting down");
        return Task.CompletedTask;
    }
    
    public class FadeRequest : IRequest
    {
        public required FadeOptions FadeOptions { get; init; }
    }

    public class FadeRequestHandler : IRequestHandler<FadeRequest>
    {
        private readonly ILogger<FadeRequestHandler> _logger;
        private readonly Fader _fader;

        public FadeRequestHandler(ILogger<FadeRequestHandler> logger, Fader fader)
        {
            _logger = logger;
            _fader = fader;
        }
        public Task Handle(FadeRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Received request to fade: {request.FadeOptions}");
            var task = _fader.FadeAsync(request.FadeOptions.WaitTime, request.FadeOptions.FadeTime, cancellationToken);
            return task;
        }
    }
    
    public class FadeOptions
    {
        public TimeSpan WaitTime { get; init; } = TimeSpan.Zero;
        public required TimeSpan FadeTime { get; init; }
        public override string ToString()
        {
            return $"WaitTime: {WaitTime}, FadeTime: {FadeTime}";
        }
    }
}