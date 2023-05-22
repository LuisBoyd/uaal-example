using System.Collections.Generic;
using System.Linq;
using Core3.MonoBehaviors;
using CustomUIFramework.Organisms;
using UnityEngine;

namespace CustomUIFramework.Technical
{
    public class SliceManager : Singelton<SliceManager>
    {
        private Dictionary<Hash128, List<Slice>> viewGroupHashed_To_SliceInstanceCollection;
        private List<Slice> _slices;

        protected override void Awake()
        {
            base.Awake();
            viewGroupHashed_To_SliceInstanceCollection = new Dictionary<Hash128, List<Slice>>();
            _slices = new List<Slice>();
        }

        public void RegisterView(ViewConfig config)
        {
            Hash128 viewGroupHashed = Hash128.Compute(config.name);
            //itterate through the config do any of the currently existing slices match the inputted ones
            foreach (SliceConfig sliceConfig in config.SliceConfigList)
            {
                Hash128 itterationHashed = Hash128.Compute(sliceConfig.Slice.SliceName);
                if (_slices.Any(s => Hash128.Compute(s.SliceName) == itterationHashed))
                {
                    if(viewGroupHashed_To_SliceInstanceCollection.ContainsKey(viewGroupHashed))
                        viewGroupHashed_To_SliceInstanceCollection[viewGroupHashed].Add(_slices.First(
                            x => Hash128.Compute(x.SliceName) == itterationHashed));
                    else
                    {
                        viewGroupHashed_To_SliceInstanceCollection.Add(viewGroupHashed, new List<Slice>()
                        {
                            _slices.First(
                                x => Hash128.Compute(x.SliceName) == itterationHashed)
                        });
                    }
                    continue;
                }
                InstantiateSlice(viewGroupHashed, sliceConfig);
            }
            
        }
        private void InstantiateSlice(Hash128 hash, SliceConfig config)
        {
            Slice sliceobj = Instantiate(config.Slice, this.transform);
            if(!viewGroupHashed_To_SliceInstanceCollection.ContainsKey(hash))
                viewGroupHashed_To_SliceInstanceCollection.Add(hash, new List<Slice>(){sliceobj});
            else
            {
                viewGroupHashed_To_SliceInstanceCollection[hash].Add(sliceobj);
            }
            _slices.Add(sliceobj);
        }

        public List<Slice> GatherSlices(ViewConfig config)
        {
            Hash128 viewHashed = Hash128.Compute(config.name);
            if(viewGroupHashed_To_SliceInstanceCollection.ContainsKey(viewHashed))
            {
                return viewGroupHashed_To_SliceInstanceCollection[viewHashed];
            }

            return null;
        }

    }
}