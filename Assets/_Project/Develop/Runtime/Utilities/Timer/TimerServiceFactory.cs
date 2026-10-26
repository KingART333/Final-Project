using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Project.Develop.Runtime.Utilities.Timer
{
    public class TimerServiceFactory
    {
        private readonly DIContainer _container;

        public TimerServiceFactory(DIContainer container)
        {
            _container = container;
        }

        public TimerService Create(float cooldown) => new TimerService(cooldown, _container.Resolve<ICoroutinesPerformer>());
    }
}
