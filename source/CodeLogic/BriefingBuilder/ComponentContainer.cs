using ArbitraryPixel.Platform2D.Engine;
using System;

namespace BriefingBuilder
{
    public class ComponentContainer : IComponentContainer
    {
        public static ComponentContainer Create() { return new ComponentContainer(new SimpleComponentContainer()); }

        private IComponentContainer _container;

        public ComponentContainer(IComponentContainer container)
        {
            _container = container ?? throw new ArgumentNullException();
        }

        #region IComponentContainer Implementation
        public bool ContainsComponent(Type componentType)
        {
            return _container.ContainsComponent(componentType);
        }

        public bool ContainsComponent<TComponent>() where TComponent : class
        {
            return _container.ContainsComponent<TComponent>();
        }

        public TComponent GetComponent<TComponent>() where TComponent : class
        {
            return _container.GetComponent<TComponent>();
        }

        public object GetComponent(Type componentType)
        {
            return _container.GetComponent(componentType);
        }

        public void RegisterComponent<TComponent>(TComponent component) where TComponent : class
        {
            _container.RegisterComponent<TComponent>(component);
        }
        #endregion
    }
}
