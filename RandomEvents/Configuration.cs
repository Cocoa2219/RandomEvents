using System.ComponentModel;
using Exiled.API.Interfaces;
using RandomEvents.API;

namespace RandomEvents
{
    public class Configuration : IConfig
    {
        [Description("플러그인 활성 여부를 설정합니다.")]
        public bool IsEnabled { get; set; } = true;

        [Description("디버그 모드를 켭니다. (잡다한 로그가 출력됩니다.)")]
        public bool Debug { get; set; } = false;

        [Description("이벤트 목록과 그 확률을 설정합니다.")]
        public EventConfiguration EventConfiguration { get; set; } = new EventConfiguration();
    }
}