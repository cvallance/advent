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
use crate::shared::calc_distance;
use itertools::Itertools;
use tracing::info;

pub fn process(input: &str) -> miette::Result<u64, AocError> {
    // let input = include_str!("../test_input.txt");
    let (time, record) = parse_values(input);

    let start = time / 2;
    // This is an assumption that you break the record at the mid point
    assert!(calc_distance(start, time) > record);

    let mut first = 0;
    let mut lower = 0;
    let mut higher = start;
    loop {
        let idx = (lower + higher) / 2;
        let distance = calc_distance(idx, time);
        if distance > record {
            higher = idx;
        } else {
            lower = idx;
        }

        if higher - lower == 1 {
            if calc_distance(lower, time) > record {
                first = lower;
            } else {
                first = higher;
            }
            break;
        }
    }

    let mut last = time;
    let mut lower = start;
    let mut higher = time;
    loop {
        let idx = (lower + higher) / 2;
        let distance = calc_distance(idx, time);
        if distance < record {
            higher = idx;
        } else {
            lower = idx;
        }

        if higher - lower == 1 {
            if calc_distance(lower, time) > record {
                last = lower;
            } else {
                last = higher;
            }
            break;
        }
    }

    let result = last - first + 1;
    Ok(result)
}

pub fn parse_values(input: &str) -> (u64, u64) {
    let mut lines = input.lines().into_iter();
    let time: u64 = lines
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
        assert_eq!(71503, process(input)?);
        Ok(())
    }
}
