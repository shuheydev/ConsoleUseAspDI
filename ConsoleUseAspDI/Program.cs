using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleUseAspDI
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            //AddSingletonの場合
            services.AddSingleton<IMyClass, MyClassA>();
            services.AddSingleton<MyTargetClass>();

            var provider = services.BuildServiceProvider();

            var myTarget = provider.GetService<MyTargetClass>();

            myTarget.PrintMessage();//I'm MyClassA
            myTarget.ChangeMessage("Hello");
            myTarget.PrintMessage();//Hello

            var myTarget2 = provider.GetService<MyTargetClass>();

            //Singletonなので同じインスタンスが使われる.
            myTarget2.PrintMessage();//Hello


            Console.WriteLine();

            //AddScopedの場合1
            services = new ServiceCollection();

            services.AddScoped<IMyClass, MyClassA>();
            services.AddSingleton<MyTargetClass>();

            provider = services.BuildServiceProvider();
            myTarget = provider.GetService<MyTargetClass>();

            myTarget.PrintMessage();//MyClassA
            myTarget.ChangeMessage("Wow");
            myTarget.PrintMessage();//WoW

            myTarget2 = provider.GetService<MyTargetClass>();

            myTarget2.PrintMessage();//WoW


            Console.WriteLine();

            //AddScopedの場合2
            services = new ServiceCollection();

            services.AddScoped<IMyClass, MyClassA>();
            services.AddScoped<MyTargetClass>();

            provider = services.BuildServiceProvider();
            myTarget = provider.GetService<MyTargetClass>();

            myTarget.PrintMessage();//MyClassA
            myTarget.ChangeMessage("Wow");
            myTarget.PrintMessage();//WoW

            myTarget2 = provider.GetService<MyTargetClass>();

            myTarget2.PrintMessage();//WoW


            Console.WriteLine();

            //AddTransientの場合1
            services = new ServiceCollection();

            services.AddTransient<IMyClass, MyClassA>();
            services.AddTransient<MyTargetClass>();

            provider = services.BuildServiceProvider();
            myTarget = provider.GetService<MyTargetClass>();

            myTarget.PrintMessage();//MyClassA
            myTarget.ChangeMessage("Wow");
            myTarget.PrintMessage();//WoW

            myTarget2 = provider.GetService<MyTargetClass>();

            myTarget2.PrintMessage();//WoW



            Console.WriteLine();

            //AddTransientの場合2
            services = new ServiceCollection();

            services.AddSingleton<IMyClass, MyClassA>();
            services.AddTransient<MyTargetClass>();

            provider = services.BuildServiceProvider();
            myTarget = provider.GetService<MyTargetClass>();

            myTarget.PrintMessage();//MyClassA
            myTarget.ChangeMessage("Wow");
            myTarget.PrintMessage();//WoW

            myTarget2 = provider.GetService<MyTargetClass>();

            myTarget2.PrintMessage();//WoW



            Console.WriteLine();

            //AddTransientの場合3
            services = new ServiceCollection();

            services.AddTransient<IMyClass, MyClassA>();
            services.AddSingleton<MyTargetClass>();

            provider = services.BuildServiceProvider();
            myTarget = provider.GetService<MyTargetClass>();

            myTarget.PrintMessage();//MyClassA
            myTarget.ChangeMessage("Wow");
            myTarget.PrintMessage();//WoW

            myTarget2 = provider.GetService<MyTargetClass>();

            myTarget2.PrintMessage();//WoW


            var scopeFactory = provider.GetService<IServiceScopeFactory>();

            var a = provider.GetRequiredService<MyTargetClass>();
            var b = provider.GetService<MyTargetClass>();
            
            if(a==b)
            {
                Console.WriteLine("true");
            }
            else
            {
                Console.WriteLine("false");
            }

            var b2 = provider.GetService<MyClassB>();
            var a1 = provider.GetRequiredService<MyClassB>();


            var serviceCollection2 = new ServiceCollection();

        }


    }

    interface IMyClass
    {
        string Message { get; set; }
    }

    class MyClassA : IMyClass
    {
        public MyClassA()
        {
            this.Message = "MyClassA";
        }
        public string Message { get; set; }
    }

    class MyClassB : IMyClass
    {
        public MyClassB()
        {
            this.Message = "I'm MyClassB";
        }
        public string Message { get; set; }
    }

    class MyTargetClass
    {
        private readonly IMyClass dependency;

        public MyTargetClass(IMyClass dependency)
        {
            Console.WriteLine(dependency.Message);
            this.dependency = dependency;
        }

        public void ChangeMessage(string newMessage)
        {
            dependency.Message = newMessage;
        }

        public void PrintMessage()
        {
            Console.WriteLine(dependency.Message);
        }
    }
}
