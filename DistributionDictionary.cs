using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentFeedback_SpaceModules
{
    class DistributionDictionary
    {
        //Basic function of this custom dictionary is to provide searchable hashsets for my performance statistics feedback app
        private HashSet<Tuple<String, String, String, Distribution>> hashSet;

        //Intended use for the keys is: roc, group, communicationskill, distribution
        public void Update(String key1, String key2, String key3, Distribution distribution)
        {
            Tuple<String, String, String, Distribution> input = new Tuple<String, String, String, Distribution>(key1, key2, key3, distribution);
            bool updated = false;
            foreach (Tuple<String, String, String, Distribution> item in hashSet)
            {
                //If this exact key sequence already exists, replace the distribution
                if (item.Item1.Equals(key1) && item.Item2.Equals(key2) && item.Item3.Equals(key3))
                {
                    item.Item4 = distribution;
                }
            }
            if (!updated)
            {
                hashSet.Add(input);
            }
        }

        public Distribution getValueByKey(String key)
        {
            return hashSet.get
        }
    }
}
