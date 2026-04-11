using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Managers.Sound
{
    public interface IMusicStorage
    {
        float LoadVolume();
        void SaveVolume(float volume);
    }
}
