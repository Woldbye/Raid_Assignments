using System.Collections.Generic;

namespace Readers 
{
  // TO:DO MAKE GENERIC INTERFACE
  interface ITextInfo<T>
  {
    IList<T> getAllInfo();

    T getInfo(int i);
  } 
}