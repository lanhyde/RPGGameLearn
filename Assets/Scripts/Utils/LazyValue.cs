using System;

namespace RPG.Utils
{
    /// <summary>
    /// Container class that wraps a value and ensures initialization is 
    /// called just before first use.
    /// </summary>
    [Serializable]
    public class LazyValue<T>
    {
        private T _value;
        private bool _initialized = false;
        private InitializerDelegate _initializer;

        public delegate T InitializerDelegate();

        /// <summary>
        /// Setup the container but don't initialise the value yet.
        /// </summary>
        /// <param name="initializer"> 
        /// The initialiser delegate to call when first used. 
        /// </param>
        public LazyValue(InitializerDelegate initializer)
        {
            _initializer = initializer;
        }

        /// <summary>
        /// Get or set the contents of this container.
        /// </summary>
        /// <remarks>
        /// Note that setting the value before initialisation will initialise 
        /// the class.
        /// </remarks>
        public T value
        {
            get
            {
                // Ensure we init before returning a value.
                ForceInit();
                return _value;
            }
            set
            {
                // Don't use default init anymore.
                _initialized = true;
                _value = value;
            }
        }

        public static implicit operator T(LazyValue<T> myInstance)
        {
            return myInstance.value;
        }

        public static implicit operator bool(LazyValue<T> myInstance)
        {
            return myInstance._initialized;
        }
        /// <summary>
        /// Force the initialization of the value via the delegate.
        /// </summary>
        public void ForceInit()
        {
            if (!_initialized)
            {
                _value = _initializer();
                _initialized = true;
            }
        }
    }
}