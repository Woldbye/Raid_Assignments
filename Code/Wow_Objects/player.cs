using Utilities;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Enumerator;

namespace Wow_Objects
{
  public class Player
  { 
    private Byte[] nameID;
    private byte info;
    // Constructor
    // Int = interrupter
    public Player(string name, Wow_Class class_, Role role_,
                    bool isOT, bool isInt, bool isAdmin) 
    { 
      this.nameID = Strings.Hash(name);
      this.info = (byte) 0; 
      this.setAdmin(isAdmin);
      this.setOT(isOT);
      this.setInterrupt(isInt);
      this.setClass(class_);
      this.setRole(role_);
      #if (DEBUG)
        Console.WriteLine("---Player init complete, printing info:---");
        Console.WriteLine(this.ToString());
      #endif      
    }

    public string getName()
    {
      return Strings.HashToString(this.nameID);
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

    // Function to extract k bits from p position 
    // and returns the extracted value as byte 
    // public static byte bitExtracted(byte number, int k, int p) 
    // first 3 bits
    public Wow_Class getClass() 
    {
      return (Wow_Class) Bytes.bitExtracted(this.info, 3, 5+1);
    }

    public Role getRole() {
      return (Role) Bytes.bitExtracted(this.info, 2, 3+1);
    }

    // index 6
    public bool isOT() 
    {
      return Convert.ToBoolean(this.info & (1 << 1));
    }

    public bool isAdmin()
    {
      return Convert.ToBoolean(this.info & 1);
    }

    public bool isInterrupter() 
    {
      return Convert.ToBoolean(this.info & (1 << 2));
    }

    // class is the first/last 3 bits, lets try last :)
    public void setClass(Wow_Class class_) 
    {
      this.info = Bytes.setBitsToNum(this.info, (byte) class_, (byte) 5, (byte) 7);
    }

    // role 3 to 4 index
    public void setRole(Role role_) 
    {
      this.info =  Bytes.setBitsToNum(this.info, (byte) role_, (byte) 3, (byte) 4);
    }
      
    public void setInterrupt(bool interrupt) 
    {
      this.info = Bytes.modifyBit(this.info, 2, interrupt);
    }

    public void setAdmin(bool admin) 
    {
        this.info = Bytes.modifyBit(this.info, 0, admin);
    }

    public void setOT(bool OT) 
    {
      this.info = Bytes.modifyBit(this.info, 1, OT);
    }

    public override string ToString() 
    {
      string ret = "<Player>";
      #if (DEBUG)
        ret += "\n\t<Binary Representation>\n\t\t" + this.info.ToBinString();
      #endif
      ret += "\n\t<Name>\n\t\t" + Strings.HashToString(this.nameID); // return string
      ret += "\n\t<Class>\n\t\t" + this.getClass().ToString();
      ret += "\n\t<Role>\n\t\t" + this.getRole().ToString();
      ret += "\n\t<Interrupter>\n\t\t" + (this.isInterrupter() ? "Yes" : "No");
      ret += "\n\t<Off-Tank>\n\t\t" + (this.isOT() ? "Yes" : "No");
      ret += "\n\t<Admin>\n\t\t" + (this.isAdmin() ? "Yes" : "No");
      return ret;
    }
  }
}