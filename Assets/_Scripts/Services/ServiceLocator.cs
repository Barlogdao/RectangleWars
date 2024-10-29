using System;
using System.Collections.Generic;


namespace RB.Services.ServiceLocator
{

    public class ServiceLocator<T>: IServiceLocator<T>
    {
        protected Dictionary<Type,T> Services;
        public ServiceLocator()
        {
            Services = new Dictionary<Type,T>();
        }

        /// <summary>
        /// Регистрация сервиса в локаторе служб
        /// </summary>
        /// <typeparam name="ST"> тип регистрируемого сервиса </typeparam>
        /// <param name="newService"> ссылка на объект регистрируемого сервиса </param>
        /// <returns> объект зарегистрированного сервиса </returns>
        public ST Register<ST>(ST newService) where ST : T
        {
            var type = newService.GetType();
            if (Services.ContainsKey(type))
            {
                throw new Exception($"Нельзя зарегистрировать сервис {type}, так как он уже зарегистрирован в локаторе служб ");
            }
            Services[type] = newService;
            return newService;
        }

        /// <summary>
        /// удаление сервиса из списка служб
        /// </summary>
        /// <typeparam name="ST"> тип сервиса </typeparam>
        /// <param name="service"> ссылка на объект удаляемого сервиса </param>
        public void Unregister<ST>(ST service) where ST : T
        {
            var type = service.GetType();
            if (Services.ContainsKey(type))
            {
                Services.Remove(type);
            }
        }

        /// <summary>
        /// получение сервиса из службы
        /// </summary>
        /// <typeparam name="ST"> тип сервиса </typeparam>
        /// <returns> объект сервиса указанного типа </returns>

        public ST Get<ST>() where ST : T
        {
            var type = typeof(ST);
            if (!Services.ContainsKey(type))
            {
                throw new Exception($"В локаторе служб отсутствует сервис типа {type}");
            }
            return (ST)Services[type];
        }

    }
}