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
        /// ����������� ������� � �������� �����
        /// </summary>
        /// <typeparam name="ST"> ��� ��������������� ������� </typeparam>
        /// <param name="newService"> ������ �� ������ ��������������� ������� </param>
        /// <returns> ������ ������������������� ������� </returns>
        public ST Register<ST>(ST newService) where ST : T
        {
            var type = newService.GetType();
            if (Services.ContainsKey(type))
            {
                throw new Exception($"������ ���������������� ������ {type}, ��� ��� �� ��� ��������������� � �������� ����� ");
            }
            Services[type] = newService;
            return newService;
        }

        /// <summary>
        /// �������� ������� �� ������ �����
        /// </summary>
        /// <typeparam name="ST"> ��� ������� </typeparam>
        /// <param name="service"> ������ �� ������ ���������� ������� </param>
        public void Unregister<ST>(ST service) where ST : T
        {
            var type = service.GetType();
            if (Services.ContainsKey(type))
            {
                Services.Remove(type);
            }
        }

        /// <summary>
        /// ��������� ������� �� ������
        /// </summary>
        /// <typeparam name="ST"> ��� ������� </typeparam>
        /// <returns> ������ ������� ���������� ���� </returns>

        public ST Get<ST>() where ST : T
        {
            var type = typeof(ST);
            if (!Services.ContainsKey(type))
            {
                throw new Exception($"� �������� ����� ����������� ������ ���� {type}");
            }
            return (ST)Services[type];
        }

    }
}