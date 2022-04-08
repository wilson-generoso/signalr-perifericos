using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Comum
{
    public interface IHubController : IAsyncDisposable
    {
        Task Initialize(IConfiguration configuration, IServiceProvider serviceProvider);
        void ConfigureRoute(IEndpointRouteBuilder routeBuilder);
    }
}
