using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RayTracor.RayTracorLib.Lights
{
    public abstract class ILight
    {
        // types of light: 
        // point light, area light, spotlight, sun, hemisphere

        public abstract void Serialize(XmlDocument doc, XmlNode parent);
    }
}