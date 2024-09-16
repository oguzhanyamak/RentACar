using Core.Application.Pipelines.Validation;
using Core.Application.Rules;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Application
{
    public static class ApplicationServiceRegistiration
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


            services.AddMediatR(configuration => 
            { 
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            }
            );
            return services;
        }



        /*
            belirtilen bir Assembly içindeki, verilen bir Type (tip) sınıfının alt sınıflarını (subclass) bulur ve bunları IServiceCollection'a ekler.
            Böylece bu alt sınıflar, bağımlılık enjeksiyonuna (DI) dahil edilip uygulamada kullanılabilir hale gelir.
        */
        public static IServiceCollection AddSubClassesOfType(
           this IServiceCollection services,
           Assembly assembly,
           Type type,
           Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null
        )
        {
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
            foreach (var item in types)
                if (addWithLifeCycle == null)
                    services.AddScoped(item);

                else
                    addWithLifeCycle(services, type);
            return services;
        }
    }
}
