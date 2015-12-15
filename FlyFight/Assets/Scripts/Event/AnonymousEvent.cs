using System;
using System.Collections.Generic;

namespace AnonymousEvent
{
	/// <summary>
	/// handler delegate used by AnonymousEventManager
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="sender"></param>
	/// <param name="arg"></param>
	public delegate void AnonymousEventHandler<T>(Object sender, T arg) where T : EventArgs;

	/// <summary>
	/// Provide anonymous event source.
	/// The internal "event" keyword provide "named" event source. Listeners must can see the event variable.
	/// While with this class, listeners only need known the event argument type.
	/// Type of event argument serves as matching key.
	/// </summary>
	public class AnonymousEventManager
	{
		private static AnonymousEventManager m_instance = null;
		public static AnonymousEventManager Instance
		{
			get {
				m_instance = m_instance == null ? new AnonymousEventManager() : m_instance;
				return m_instance;
			}
		}
		/// <summary>
		/// Raise event, all handler registered with argument type T will receive the event
		/// </summary>
		/// <typeparam name="T">Can not be EventArgs</typeparam>
		/// <param name="sender"></param>
		/// <param name="arg"></param>
		public void RaiseEvent<T>(object sender, T arg) where T : EventArgs
		{
			CheckNullReference(arg, "arg");

			if (typeof(T) == typeof(EventArgs))
				throw new Exception("T can not be EventArgs");

			HandlerChain<T> handlerChain = FindHandlerChain<T>();
			if (handlerChain != null)
				handlerChain.Invoke(sender, arg);
		}

		/// <summary>
		/// Raise event, all handler registered with any type that is in arg's type's inherit branch will receive the event
		/// The invokde sequence is from base type to inherited type
		/// e.g ArgType3 extends ArgType2, ArgType2 extends ArgType1, Arg1 extends EventArgs. 
		/// RaiseEventIncludingBase with ArgType3 equivalent to: RaiseEvent with ArgType1, ArgType2 and ArgType3
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="arg"></param>
		public void RaiseEventIncludingBase(object sender, EventArgs arg)
		{
			CheckNullReference(arg, "arg");
			if (arg.GetType() == typeof(EventArgs))
				throw new Exception("typeof arg can not be EventArgs");

			RaiseEventIncludingBaseWithHandlerType(sender, arg, arg.GetType());
		}

		private void RaiseEventIncludingBaseWithHandlerType(object sender, EventArgs arg, Type handlerArgType)
		{
			//recursively RaiseEvent for base type
			Type handlerArgBaseType = handlerArgType.BaseType;
			if (handlerArgBaseType != typeof(EventArgs))
				RaiseEventIncludingBaseWithHandlerType(sender, arg, handlerArgBaseType);

			if (!handlerArgType.IsAbstract)
			{
				HandlerChainBase handler = FindHandlerChainBase(handlerArgType);
				if (handler != null)
					handler.InvokeNonGeneric(sender, arg);
			}
		}

		/// <summary>
		/// Register and handler with argment type as T
		/// </summary>
		/// <typeparam name="T">Can not be EventArgs</typeparam>
		/// <param name="f"></param>
		public void AddHandler<T>(AnonymousEventHandler<T> handler) where T : EventArgs
		{
			CheckNullReference(handler, "handler");

			if (typeof(T) == typeof(EventArgs))
				throw new Exception("T can not be EventArgs");

			HandlerChain<T> handlerChain = RequireHandlerChain<T>();
			handlerChain.AddHandler(handler);
		}

		/// <summary>
		/// Remove and handler with argment type as T
		/// </summary>
		/// <typeparam name="T">Can not be EventArgs</typeparam>
		/// <param name="f"></param>
		public void RemoveHandler<T>(AnonymousEventHandler<T> handler) where T : EventArgs
		{
			CheckNullReference(handler, "handler");

			if (typeof(T) == typeof(EventArgs))
				throw new Exception("T can not be EventArgs");

			HandlerChain<T> handlerChain = FindHandlerChain<T>();
			if (handlerChain != null)
				handlerChain.RemoveHandler(handler);
		}

		/// <summary>
		/// Clear handlers with argument type T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public void Clear<T>()
		{
			m_handlerChainMap.Remove(typeof(T));
		}

		/// <summary>
		/// Clear all handlers
		/// </summary>
		public void ClearAll()
		{
			m_handlerChainMap.Clear();
		}

		private abstract class HandlerChainBase
		{
			public abstract void InvokeNonGeneric(Object sender, Object arg);
		}

		private class HandlerChain<T> : HandlerChainBase where T : EventArgs
		{
			public override void InvokeNonGeneric(Object sender, Object arg)
			{
				Invoke(sender, (T)arg);
			}

			public void Invoke(Object sender, T arg)
			{

				if (m_delegateChain != null)
					m_delegateChain(sender, arg);
			}

			public void AddHandler(AnonymousEventHandler<T> handler)
			{
				m_delegateChain += handler;
			}

			public void RemoveHandler(AnonymousEventHandler<T> handler)
			{
				m_delegateChain -= handler;
			}

			AnonymousEventHandler<T> m_delegateChain = null;
		}

		private Dictionary<Type, HandlerChainBase> m_handlerChainMap = new Dictionary<Type, HandlerChainBase>();
		private HandlerChainBase FindHandlerChainBase(Type type)
		{
			HandlerChainBase handlerChainBase;
			if (m_handlerChainMap.TryGetValue(type, out handlerChainBase))
			{
				return handlerChainBase;
			}
			else
			{
				return null;
			}
		}
		private HandlerChain<T> FindHandlerChain<T>() where T : EventArgs
		{
			HandlerChainBase handlerChainBase;
			if (m_handlerChainMap.TryGetValue(typeof(T), out handlerChainBase))
			{
				return (HandlerChain<T>)handlerChainBase;
			}
			else
			{
				return null;
			}
		}
		private HandlerChain<T> RequireHandlerChain<T>() where T : EventArgs
		{
			HandlerChainBase handlerChainBase;
			if (m_handlerChainMap.TryGetValue(typeof(T), out handlerChainBase))
			{
				return (HandlerChain<T>)handlerChainBase;
			}
			else
			{
				HandlerChain<T> newChain = new HandlerChain<T>();
				m_handlerChainMap.Add(typeof(T), newChain);
				return newChain;
			}
		}
		private static void CheckNullReference(Object reference, String name)
		{
			if (reference == null)
				throw new NullReferenceException(name);
		}
	}
}	//end namespace AnonymousEvent
