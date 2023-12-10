pub fn parse_input(input: &str) -> Vec<Vec<i64>> {
    input
        .lines()
        .map(|line| {
            line.split_whitespace()
                .map(|num| num.parse::<i64>().expect("Failed to parse integer"))
                .collect::<Vec<i64>>()
        })
        .collect()
}
