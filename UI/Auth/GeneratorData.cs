namespace UI.Auth {
    public static class GeneratorData {
        public static string GenerateTwoFactorCode () {
            return new Random ().Next (0, 1000000).ToString ("D6");
        }
    }
}
