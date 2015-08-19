using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracor.RayTracorLib.Tracing;

namespace RayTracor.RayTracorLib.Utility
{
    public class PoissonDisk2
    {
        List<Vector2> samples;

        double r;
        int k = 30;
        int ri = 0;
        List<int> lastRandomSamples;

        public Vector2[] Samples { get { return samples.ToArray(); } }

        public Vector2 RandomSample { get { ri = (ri + 1) % samples.Count; return samples[ri]; } }

        public PoissonDisk2(double r)
        {
            samples = new List<Vector2>();
            lastRandomSamples = new List<int>();
            this.r = r;
        }

        public Vector2 GetRandomSample()
        {
            int rsi = MwcRng.GetInt(samples.Count);
            while (lastRandomSamples.Contains(rsi))
                rsi = MwcRng.GetInt(samples.Count);
            lastRandomSamples.Add(rsi);
            while (lastRandomSamples.Count > samples.Count / 2)
                lastRandomSamples.RemoveAt(0);
            return samples[rsi];
        }

        public void Generate()
        {
            samples.Clear();

            Stopwatch sw = Stopwatch.StartNew();

            // step0 -> init background grid
            double cellsize = r / Math.Sqrt(2);
            double domainSize = 1.0;

            // grid
            int gridsize = (int)(domainSize / cellsize) + 1;
            int[] grid = new int[gridsize * gridsize];
            for (int i = 0; i < grid.Length; i++)
                grid[i] = -1;

            List<Vector2> active = new List<Vector2>();

            // step1 -> initial sample
            MwcRng.SeedFromTime();

            Vector2 v0 = new Vector2(MwcRng.GetUniformRange(0, domainSize), MwcRng.GetUniformRange(0, domainSize));
            int x = (int)(v0.X / cellsize);
            int y = (int)(v0.Y / cellsize);
            grid[x + y * gridsize] = 0;
            samples.Add(v0);
            active.Add(v0);

            int gridradius = (int)Math.Ceiling(2 * r / cellsize);

            // step2
            while (active.Count > 0)
            {
                // random index from active list
                int i = MwcRng.GetInt(active.Count);
                Vector2 vi = active[i];

                // generate up to k random points
                int j;
                for (j = 0; j < k; j++)
                {
                    double rad = MwcRng.GetUniformRange(r, 2 * r);
                    double angle = MwcRng.GetUniformRange(0, 2 * Math.PI);

                    Vector2 newvec = vi + Vector2.FromAngle(angle) * rad;

                    // check if valid
                    if (newvec.X < 0 || newvec.Y < 0 || newvec.X > domainSize || newvec.Y > domainSize)
                        continue;
                    int gx = (int)(newvec.X / cellsize);
                    int gy = (int)(newvec.Y / cellsize);
                    if (grid[gx + gy * gridsize] > -1)
                        continue;

                    int gxstart = Math.Max(0, gx - gridradius);
                    int gystart = Math.Max(0, gy - gridradius);
                    int gxend = Math.Min(gridsize - 1, gx + gridradius);
                    int gyend = Math.Min(gridsize - 1, gy + gridradius);

                    bool add = true;

                    for (int xx = gxstart; xx <= gxend; xx++)
                    {
                        for (int yy = gystart; yy <= gyend; yy++)
                        {
                            if (grid[xx + yy * gridsize] > -1)
                            {
                                Vector2 gv = samples[grid[xx + yy * gridsize]];
                                double dist = (gv - newvec).Length;
                                if (dist <= r)
                                {
                                    add = false;
                                    break;
                                }
                            }
                        }
                        if (!add)
                            break;
                    }

                    if (add)
                    {
                        grid[gx + gy * gridsize] = samples.Count;
                        samples.Add(newvec);
                        active.Add(newvec);
                        break;
                    }
                }
                if (j >= k)
                    active.RemoveAt(i);
            }
            sw.Stop();

            lastRandomSamples.Clear();
            ri = MwcRng.GetInt(samples.Count);

            Console.WriteLine("finished in {0}ms", sw.ElapsedMilliseconds);
        }
    }
}
