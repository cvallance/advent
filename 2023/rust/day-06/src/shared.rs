pub fn calc_distance(hold_time: u64, race_time: u64) -> u64 {
    hold_time * (race_time - hold_time)
}
