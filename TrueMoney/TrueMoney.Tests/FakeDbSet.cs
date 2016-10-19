// https://gist.github.com/taschmidt/9663503

// also we can use package from nuget, but as i can see on github, it is not async https://www.nuget.org/packages/FakeDbSet/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TrueMoney.Tests
{
    public class FakeDbSet<T> : IDbSet<T>, IDbAsyncEnumerable<T> where T : class
    {
        readonly ObservableCollection<T> _data;
        readonly IQueryable _queryable;

        public FakeDbSet()
        {
            _data = new ObservableCollection<T>();
            _queryable = _data.AsQueryable();
        }

        public virtual T Find(params object[] keyValues)
        {
            throw new NotImplementedException("Derive from FakeDbSet<T> and override Find");
        }

        public Task<T> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public T Add(T item)
        {
            _data.Add(item);
            return item;
        }

        public T Remove(T item)
        {
            _data.Remove(item);
            return item;
        }

        public T Attach(T item)
        {
            _data.Add(item);
            return item;
        }

        public T Detach(T item)
        {
            _data.Remove(item);
            return item;
        }

        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public ObservableCollection<T> Local
        {
            get { return _data; }
        }

        Type IQueryable.ElementType
        {
            get { return _queryable.ElementType; }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get { return _queryable.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new AsyncQueryProviderWrapper<T>(_queryable.Provider); }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public int Count
        {
            get { return _data.Count; }
        }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new AsyncEnumeratorWrapper<T>(_data.GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }
    }

    internal class AsyncQueryProviderWrapper<T> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal AsyncQueryProviderWrapper(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new AsyncEnumerableQuery<T>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new AsyncEnumerableQuery<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }

    public class AsyncEnumerableQuery<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable
    {
        public AsyncEnumerableQuery(IEnumerable<T> enumerable) : base(enumerable)
        {
        }

        public AsyncEnumerableQuery(Expression expression) : base(expression)
        {
        }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new AsyncEnumeratorWrapper<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new AsyncQueryProviderWrapper<T>(this); }
        }
    }

    public class AsyncEnumeratorWrapper<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public AsyncEnumeratorWrapper(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }

    // without async
    // https://gist.github.com/LukeWinikates/1309447
    //public class FakeDbSet<T> : System.Data.Entity.IDbSet<T> where T : class
    //{
    //    private readonly List<T> list = new List<T>();

    //    public FakeDbSet()
    //    {
    //        list = new List<T>();
    //    }

    //    public FakeDbSet(IEnumerable<T> contents)
    //    {
    //        this.list = contents.ToList();
    //    }

    //    #region IDbSet<T> Members

    //    public T Add(T entity)
    //    {
    //        this.list.Add(entity);
    //        return entity;
    //    }

    //    public T Attach(T entity)
    //    {
    //        this.list.Add(entity);
    //        return entity;
    //    }

    //    public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public T Create()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public T Find(params object[] keyValues)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public System.Collections.ObjectModel.ObservableCollection<T> Local
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    public T Remove(T entity)
    //    {
    //        this.list.Remove(entity);
    //        return entity;
    //    }

    //    #endregion

    //    #region IEnumerable<T> Members

    //    public IEnumerator<T> GetEnumerator()
    //    {
    //        return this.list.GetEnumerator();
    //    }

    //    #endregion

    //    #region IEnumerable Members

    //    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    //    {
    //        return this.list.GetEnumerator();
    //    }

    //    #endregion

    //    #region IQueryable Members

    //    public Type ElementType
    //    {
    //        get { return this.list.AsQueryable().ElementType; }
    //    }

    //    public System.Linq.Expressions.Expression Expression
    //    {
    //        get { return this.list.AsQueryable().Expression; }
    //    }

    //    public IQueryProvider Provider
    //    {
    //        get { return this.list.AsQueryable().Provider; }
    //    }

    //    #endregion
    //}

    ////public class FakeDbSetFromBlogPost<T> : IDbSet<T>
    ////where T : class
    ////{
    ////    HashSet<T> _data;
    ////    IQueryable _query;

    ////    public FakeDbSetFromBlogPost()
    ////    {
    ////        _data = new HashSet<T>();
    ////        _query = _data.AsQueryable();
    ////    }

    ////    public virtual T Find(params object[] keyValues)
    ////    {
    ////        throw new NotImplementedException("Derive from FakeDbSet<T> and override Find");
    ////    }
    ////    public T Add(T item)
    ////    {
    ////        _data.Add(item);
    ////        return item;
    ////    }

    ////    public T Remove(T item)
    ////    {
    ////        _data.Remove(item);
    ////        return item;
    ////    }

    ////    public T Attach(T item)
    ////    {
    ////        _data.Add(item);
    ////        return item;
    ////    }

    ////    public void Detach(T item)
    ////    {
    ////        _data.Remove(item);
    ////    }

    ////    Type IQueryable.ElementType
    ////    {
    ////        get { return _query.ElementType; }
    ////    }

    ////    System.Linq.Expressions.Expression IQueryable.Expression
    ////    {
    ////        get { return _query.Expression; }
    ////    }

    ////    IQueryProvider IQueryable.Provider
    ////    {
    ////        get { return _query.Provider; }
    ////    }

    ////    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    ////    {
    ////        return _data.GetEnumerator();
    ////    }

    ////    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    ////    {
    ////        return _data.GetEnumerator();
    ////    }
    ////}
}
