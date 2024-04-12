using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Exiled.API.Interfaces;
using RandomEvents.API;
using RandomEvents.API.Interfaces;

namespace RandomEvents
{
    public class Configuration : IConfig
    {
        [Description("플러그인 활성 여부를 설정합니다.")]
        public bool IsEnabled { get; set; } = true;

        [Description("디버그 모드를 켭니다. (잡다한 로그가 출력됩니다.)")]
        public bool Debug { get; set; } = true;

        [Description("이벤트의 설정을 제어합니다.")]
        public Dictionary<string, IEventConfig> EventConfigs { get; set; } = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IEventConfig).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (IEventConfig)Activator.CreateInstance(t)).ToDictionary(x => x.Name, x => x);
    }
}