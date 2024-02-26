using ProjectB.io;

namespace ProjectBTest;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestAdditionCalculator()
    {
        AdditionCalculator additionCalculator = new();
        Assert.AreEqual(5, additionCalculator.Process(2, 3));
    }

    [TestMethod]
    public void TestAdditionCalculator_ShouldNotBeEqual()
    {
        AdditionCalculator additionCalculator = new();
        Assert.AreNotEqual(5, additionCalculator.Process(3, 3));
    }
}