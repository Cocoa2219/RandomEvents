using Exiled.API.Features;

namespace RandomEvents.API.Events.TestEvent;

public class TestEventHandler(TestEvent testEvent)
{
    private TestEvent TestEvent { get; set; } = testEvent;

    public void OnRoundStart()
    {
        TestEvent.LogDebug("라운드가 시작되었습니다.");
    }
}