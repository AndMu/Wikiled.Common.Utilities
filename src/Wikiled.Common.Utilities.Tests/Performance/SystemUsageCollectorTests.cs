using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Common.Utilities.Performance;

namespace Wikiled.Common.Utilities.Tests.Performance;

[TestFixture]
public class SystemUsageCollectorTests
{
    private SystemUsageCollector instance;

    [SetUp]
    public void SetUp()
    {
        instance = CreateInstance();
    }

    [Test]
    public void Collect()
    {
        instance.Refresh();
        ClassicAssert.GreaterOrEqual(instance.UserCpuUsed, 0);
    }

    private SystemUsageCollector CreateInstance()
    {
        return new SystemUsageCollector();
    }
}