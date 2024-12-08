using LearningStarter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder => builder
        .CaptureStartupErrors(true)
        .UseStartup<Startup>());

host.Build().Run();