﻿using Coolector.Common.Host;
using Coolector.Services.Remarks.Framework;
using Coolector.Services.Remarks.Shared.Commands;
using Coolector.Services.Users.Shared.Events;

namespace Coolector.Services.Remarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 10002)
                .UseAutofac(Bootstrapper.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToCommand<CreateRemark>()
                .SubscribeToCommand<DeleteRemark>()
                .SubscribeToCommand<ResolveRemark>()
                .SubscribeToCommand<AddPhotosToRemark>()
                .SubscribeToCommand<RemovePhotosFromRemark>()
                .SubscribeToCommand<SubmitRemarkVote>()
                .SubscribeToCommand<DeleteRemarkVote>()
                .SubscribeToEvent<SignedUp>()
                .SubscribeToEvent<UsernameChanged>()
                .Build()
                .Run();
        }
    }
}
