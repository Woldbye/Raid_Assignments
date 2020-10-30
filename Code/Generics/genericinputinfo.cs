using System;
using System.Collections.Generic;

namespace Generics
{
  // Any child class should be implemented with Singleton pattern.
  public abstract class GenericInputInfo<TIterator> where TIterator : struct, IConvertible
  {
    private Type[] _inputTypes;
    private int _count;

    protected GenericInputInfo()
    {
      if (!typeof(TIterator).IsEnum)
      {
         throw new ArgumentException(String.Format("generic type {0} must be an enumerated type", typeof(TIterator)));
      }
      this._count = Enum.GetValues(typeof(TIterator)).Length;
      this._inputTypes = new Type[this._count];
      this.InitTypes();
    }
  
    public int Count
    {
      get { return this._count; }
      protected set { this._count = value; }
    }

    public Type[] AsArray
    {
      get { return this._inputTypes; }
    }

    // initializes _inputTypes
    public abstract void InitTypes();

    public Type this[TIterator enumIterator] 
    { 
      get
      {
        return this._inputTypes[Convert.ToInt32(enumIterator)]; 
      } 
      protected set 
      {
        int iConverted = Convert.ToInt32(enumIterator);
        this._inputTypes[iConverted] = value;
      }
    }
  }
}