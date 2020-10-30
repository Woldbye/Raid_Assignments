using System;
using System.Collections.Generic;

namespace Generics
{
  // Any child class should be implemented with Singleton pattern.
  public abstract class GenericFlags<TIterator> where TIterator : struct, IConvertible
  {
    private string[] _flags;

    protected GenericFlags()
    {
      if (!typeof(TIterator).IsEnum)
      {
         throw new ArgumentException(String.Format("generic type {0} must be an enumerated type", typeof(TIterator)));
      }
      int count = Enum.GetValues(typeof(TIterator)).Length;
      this._flags = new String[count];
      this.InitFlags();
    }
    
    public string[] AsArray
    {
      get { return this._flags; }
    }

    // initializes _flags
    public abstract void InitFlags();
  
    public string this[TIterator enumIterator] 
    { 
      get
      {
        return this._flags[Convert.ToInt32(enumIterator)]; 
      } 
      protected set 
      {
        int iConverted = Convert.ToInt32(enumIterator);
        this._flags[iConverted] = value;
      }
    }
  }
}