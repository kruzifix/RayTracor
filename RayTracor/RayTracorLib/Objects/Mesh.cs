using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RayTracor.RayTracorLib.Materials;
using RayTracor.RayTracorLib.Tracing;
using RayTracor.RayTracorLib.Utilities;

namespace RayTracor.RayTracorLib.Objects
{
    public class Mesh : IObject
    {
        List<Triangle> Triangles { get; set; }

        public Mesh(IEnumerable<Triangle> tris, string material)
            :base(material)
        {
            Triangles = new List<Triangle>();
            Triangles.AddRange(tris);
        }

        public override Intersection Intersects(Ray ray)
        {
            Intersection closestIntersec = Intersection.False;
            closestIntersec.Distance = double.MaxValue;

            foreach (Triangle tri in Triangles)
            {
                Intersection res = tri.Intersects(ray);

                if (res.Intersects && res.Distance > 0.0005 && res.Distance < closestIntersec.Distance)
                    closestIntersec = res;
            }

            return closestIntersec;
        }

        public override Vector3 EvalMaterial(Intersection intersec, Material mat)
        {
            return mat.Color.ToVector();
        }

        public static IObject FromJToken(JToken tok)
        {
            SerializedMesh sm = JsonConvert.DeserializeObject<SerializedMesh>(tok.ToString());

            if (string.IsNullOrWhiteSpace(sm.material))
                throw new Exception("Mesh: 'material' not defined.");
            if (string.IsNullOrWhiteSpace(sm.model))
                throw new Exception("Mesh: 'model' not defined.");

            if (!File.Exists(sm.model))
                throw new Exception(string.Format("Mesh: Unable to load 'model' at path '{0}'.", sm.model));

            string[] content = File.ReadAllLines(sm.model);

            return new Mesh(ParseObjFile(content, sm.material), sm.material);
        }

        private static IEnumerable<Triangle> ParseObjFile(string[] content, string mat)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();

            List<Triangle> tris = new List<Triangle>();

            foreach(string line in content)
            {
                if (line.StartsWith("#"))
                    continue;

                string[] tokens = line.Split(' ');
                if (tokens.Length == 0)
                    continue;
                switch(tokens[0])
                {
                    case "v":
                        vertices.Add(new Vector3(double.Parse(tokens[1]), double.Parse(tokens[2]), double.Parse(tokens[3])));
                        break;
                    case "vn":
                        normals.Add(new Vector3(double.Parse(tokens[1]), double.Parse(tokens[2]), double.Parse(tokens[3])));
                        break;
                    case "f":
                        string[] verts = tokens[1].Split('/');
                        Vertex v0 = new Vertex { Position = vertices[int.Parse(verts[0]) - 1] };
                        verts = tokens[2].Split('/');
                        Vertex v1 = new Vertex { Position = vertices[int.Parse(verts[0]) - 1] };
                        verts = tokens[3].Split('/');
                        Vertex v2 = new Vertex { Position = vertices[int.Parse(verts[0]) - 1] };

                        tris.Add(new Triangle(v0,v1,v2, mat));
                        break;
                }
            }

            return tris;
        }
    }

    class SerializedMesh
    {
        public string material { get; set; }
        public string model { get; set; }
    }
}