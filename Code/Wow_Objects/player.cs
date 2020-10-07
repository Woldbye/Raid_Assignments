using Utilities;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Enumerator;

namespace Wow_Objects
{
  // To:do
  // Consider making Decorator pattern for Player at some point :)
  public class Player
  { 
    private Byte[] nameID;
    private byte info;

    public string Name
    {
      get
      {
        return Strings.HashToString(this.nameID);
      }
    }

    public Wow.Class Class
    {
      get 
      { 
        return (Wow.Class) Bytes.bitExtracted(this.info, 3, 6);
      }
    }

    public Wow.Role Role
    {
      get 
      { 
        return (Wow.Role) Bytes.bitExtracted(this.info, 2, 4);
      }
    }

    public bool IsAdmin
    {
      get 
      { 
        return Convert.ToBoolean(this.info & 1);
      }
    }

    public bool IsAssigned
    {
      get
      {
        return Convert.ToBoolean(this.info & (1 << 1));
      }
      set
      {
        this.info = Bytes.modifyBit(this.info, 1, value); 
      }   
    }

    // Constructor
    // Int = interrupter
    public Player(string name, Wow.Class class_, Wow.Role role, bool isAdmin) 
    { 
      this.nameID = Strings.Hash(name);
      this.info = 0x0; 
      this.setIsAdmin(isAdmin);
      this.setClass(class_);
      this.setRole(role);
      #if (DEBUG)
        Console.WriteLine("---Player init complete, printing info:---");
        Console.WriteLine(this.ToString());
      #endif      
    }

    private void setClass(Wow.Class class_)
    {
      this.info = Bytes.setBitsToNum(this.info, (byte) class_, (byte) 5, (byte) 7);
    }

    private void setRole(Wow.Role role)
    {
      this.info =  Bytes.setBitsToNum(this.info, (byte) role, (byte) 3, (byte) 4);
    }

    private void setIsAdmin(bool isAdmin) 
    {
      this.info = Bytes.modifyBit(this.info, 0, isAdmin);
    }

    public bool Equals(string str) {
      Byte[] strID = Strings.Hash(str);
      return Bytes.ByteArrCompare(this.nameID, strID);
    }

    public override bool Equals(Object obj)
    {
      //Check for null and compare run-time types.
      if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
      {
        return false;
      } else {
        Player obj_pl = (Player) obj;
        return Bytes.ByteArrCompare(this.nameID, obj_pl.nameID);
      }
    }

    public override string ToString() 
    {
      string ret = "<Player>";
      #if (DEBUG)
        ret += "\n\t<Binary Representation>\n\t\t" + this.info.ToBinString();
      #endif
      ret += "\n\t<Name>\n\t\t" + Strings.HashToString(this.nameID); // return string
      ret += "\n\t<Class>\n\t\t" + this.Class.ToString();
      ret += "\n\t<Role>\n\t\t" + this.Role.ToString();
      ret += "\n\t<Assigned>\n\t\t" + (this.IsAssigned ? "Yes" : "No");
      ret += "\n\t<Admin>\n\t\t" + (this.IsAdmin ? "Yes" : "No");
      return ret;
    }
  }
}