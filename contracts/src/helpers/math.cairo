#[generate_trait]
impl Math of MathTrait {
    #[inline(always)]
    fn min(a: u8, b: u8) -> u8 {
        if a < b {
            return a;
        }
        b
    }
}
