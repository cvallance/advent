// As the race is about to start, you realize the piece of paper with race times and record distances you got earlier actually just has very bad kerning. There's really only one race - ignore the spaces between the numbers on each line.
//
// So, the example from before:
//
// Time:      7  15   30
// Distance:  9  40  200
// ...now instead means this:
//
// Time:      71530
// Distance:  940200
// Now, you have to figure out how many ways there are to win this single race. In this example, the race lasts for 71530 milliseconds and the record distance you need to beat is 940200 millimeters. You could hold the button anywhere from 14 to 71516 milliseconds and beat the record, a total of 71503 ways!

use crate::custom_error::AocError;
use itertools::Itertools;

pub fn process(input: &str) -> miette::Result<f64, AocError> {
    // let input = include_str!("../test_input.txt");
    let (time, record) = parse_values(input);

    let button_time = (time + (time * time - 4.0 * record).sqrt()) / 2.0;
    let optimal_time = time as f64 / 2.0;
    if time % 2.0 == 0.0 {
        return Ok(2.0 * (button_time.floor() - optimal_time.floor()) + 1.0);
    } else {
        return Ok(2.0 * (button_time.floor() - optimal_time.floor()));
    }
}

pub fn parse_values(input: &str) -> (f64, f64) {
    let mut lines = input.lines().into_iter();
    let time: f64 = lines
        .nth(0)
        .unwrap()
        .trim_start_matches("Time: ")
        .split_whitespace()
        .join("")
        .parse()
        .unwrap();
    let distance = lines
        .nth(0)
        .unwrap()
        .trim_start_matches("Distance: ")
        .split_whitespace()
        .join("")
        .parse()
        .unwrap();

    (time, distance)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        let input = include_str!("../test_input.txt");
        assert_eq!(71503.0, process(input)?);
        Ok(())
    }
}
