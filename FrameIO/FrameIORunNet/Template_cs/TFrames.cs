
using FrameIO.Run;

namespace main
{

    public static class FioRunner
    {

        static FioRunner()
        {
            var config = string.Concat(
                "H4sIAAAAAAAEAO1YW2/aMBR+n7T/EOW5Dw0sUbe3QWlVqRVIrO3DVCEXH6jV",
                "xEHBmcRQ/vucxAmksT1T6HqZz0uUc/d3fHxbf/7kOO5ZgiK4JEvmfnN+5hzH",
                "WZefSjjpX51yYc3l/DHMI6CsaVbSevuHq/bjKEIUj8lvaDppuvqxWuTy/PeC",
                "MphD4h61dcmcAuZqMxQuoS3vEdaPU5rn5AUS8YrBMMHcdR4oQmEoiTGg0xgX",
                "QdxRQiLCyC+QqN2gMM0TPm5Ksu3f7EgHzC3Bc2AXGIp8LTC1h2uaLgGPELaw",
                "bHu4D+PpI0YMGcJynsTpQgaKunelgavwniyuMrqyKJWFvjRCa6tAJ0odgyoJ",
                "VbNaCWVFxUrK2sxM4kgBZcdCuaH9oOxaKDckg/Ip6+7ZC9AI5acBBonds1rY",
                "UI6MISJDCsOZanE+hQVQPKTCrEi8CXvbpnC463Le84JjTeuUeVS4+MGXrrJl",
                "/radaLIQLuoRFvF66WxWVFVj8pzmruxMWlzoGjR6pWnc7sJgl6YXJtrWL0my",
                "AAiB0rWuMNqTWEPTVkNC6mrIBXcy9l7bY887Me/ywPN92+Xvel7ZLn9L1fhn",
                "Xe53zbu843X8r/95m0vOk01VO7PKJGP2oKvek6lF0zA0nFnGqR7u+sKI8RH9",
                "wE8nlCD6MtdUlqQf45ZqPCNXqbSIQmqh3AHKhFgka3rZtaf/ANPHzZ3+4z2c",
                "tEXFkOvxDM5HN5Px9ZUXTM6+X44H7p5QpvevgGb7DeSNvULxvZiP9bCbnHbS",
                "b35qNcErPpyZ/QEnrvX9ZhwAAA==");

            IORunner.InitialFromGZipBase64(config);
        }

    }
}
