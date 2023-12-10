// Of course, it would be nice to have even more history included in your report. Surely it's safe to just extrapolate backwards as well, right?
//
// For each history, repeat the process of finding differences until the sequence of differences is entirely zero. Then, rather than adding a zero to the end and filling in the next values of each previous sequence, you should instead add a zero to the beginning of your sequence of zeroes, then fill in new first values for each previous sequence.
//
// In particular, here is what the third example history looks like when extrapolating back in time:
//
// 5  10  13  16  21  30  45
//   5   3   3   5   9  15
//    -2   0   2   4   6
//       2   2   2   2
//         0   0   0
// Adding the new values on the left side of each sequence from bottom to top eventually reveals the new left-most history value: 5.
//
// Doing this for the remaining example data above results in previous values of -3 for the first history and 0 for the second history. Adding all three new values together produces 2.
//
// Analyze your OASIS report again, this time extrapolating the previous value for each history. What is the sum of these extrapolated values?

use crate::custom_error::AocError;
use crate::shared::parse_input;
use tracing::info;

pub fn process(input: &str) -> miette::Result<i64, AocError> {
    // let input = include_str!("../test_input.txt");
    let mut values = parse_input(input);
    let mut result = 0;
    for value in values {
        result += process_line(value);
    }

    Ok(result)
}

pub fn process_line(line: Vec<i64>) -> i64 {
    let mut all_lines = vec![line];
    let mut result = 0;
    loop {
        let mut new_line = vec![];
        let mut all_zero = true;
        let current_line = all_lines[0].clone();
        for i in 0..current_line.len() - 1 {
            let diff = current_line[i + 1] - current_line[i];
            new_line.push(diff);
            if diff != 0 {
                all_zero = false;
            }
        }
        if all_zero {
            break;
        }
        all_lines.insert(0, new_line);
    }

    let mut result = 0;
    for i in 0..all_lines.len() - 1 {
        let cur = &all_lines.clone()[i];
        let mut next = all_lines.get_mut(i + 1).unwrap();

        let value = next[0] - cur[0];
        next.insert(0, value);
        result = value;
    }

    result
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        let input = include_str!("../test_input.txt");
        assert_eq!(2, process(input)?);
        Ok(())
    }
}
