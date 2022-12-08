using AdapterWeb.Controllers;
using MultiApp.Model;

namespace test
{
    [TestClass]
    public class FacturaTest
    {
        [TestMethod]
        public void IndexTest()
        {
            FacturaController controller = new FacturaController();
            Task<IEnumerable<factura>> result = controller.OnGet() as Task<IEnumerable<factura>>;
            Assert.IsNotNull(result);

        }
    }
}
