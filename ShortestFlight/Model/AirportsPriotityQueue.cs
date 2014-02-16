using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShortestFlight
{
    public class VPriorityQueue<T> where T : airport
    {
        List<T> element_distance_pair_list;
        int limit_size = -1;
        bool is_incremental = false;


        public VPriorityQueue()
        {
            element_distance_pair_list = new List<T>();
        }

        public override string ToString()
        {
            return element_distance_pair_list.ToString();
        }


        private int bin_locate_pos(double distance, bool is_incremental)
        {
            int mid = 0;
            int low = 0;
            int high = element_distance_pair_list.Count - 1;
            //
            while (low <= high)
            {
                mid = (low + high) / 2;
                if (element_distance_pair_list.ElementAt(mid).get_distance() == distance)
                    return mid + 1;

                if (is_incremental)
                {
                    if (element_distance_pair_list.ElementAt(mid).get_distance() < distance)
                    {
                        high = mid - 1;
                    }
                    else
                    {
                        low = mid + 1;
                    }
                }
                else
                {
                    if (element_distance_pair_list.ElementAt(mid).get_distance() > distance)
                    {
                        high = mid - 1;
                    }
                    else
                    {
                        low = mid + 1;
                    }
                }
            }
            return low;
        }


        public void Add(T element)
        {
            element_distance_pair_list.Insert(bin_locate_pos(element.get_distance(), is_incremental), element);

            if (limit_size > 0 && element_distance_pair_list.Count > limit_size)
            {
                int size_of_results = element_distance_pair_list.Count;
                element_distance_pair_list.RemoveAt(size_of_results - 1);
            }
        }


        public int Size()
        {
            return element_distance_pair_list.Count;
        }


        public T Get(int i)
        {
            if (i >= element_distance_pair_list.Count)
            {
                Console.WriteLine("The result :" + i + " doesn't exist!!!");
            }
            return element_distance_pair_list[i];
        }


        public T Poll()
        {
            T ret = element_distance_pair_list[0];
            element_distance_pair_list.RemoveAt(0);
            return ret;
        }


        public bool IsEmpty()
        {
            return (element_distance_pair_list.Count == 0);
        }

        public void Clear()
        {
            element_distance_pair_list.Clear();
        }
    }
}