use crate::custom_error::AocError;
use crate::shared::{create_full_regex_pattern, parse_input};
use regex::Regex;
use std::error::Error;
use std::sync::{Arc, Mutex};
use std::thread;
use tracing::{error, info};

pub fn process(input: &str) -> miette::Result<u64, AocError> {
    // let input = include_str!("../test_input.txt");
    // let input = include_str!("../test_input_two.txt");

    let result: Arc<Mutex<u64>> = Arc::new(Mutex::new(0));
    let finished: Arc<Mutex<u64>> = Arc::new(Mutex::new(0));
    let mut handles = vec![];

    let items = parse_input(input);
    let total = items.len();
    for (orig_pattern, groups) in items {
        // let orig_pattern = orig_pattern.clone();
        let pattern = std::iter::repeat(orig_pattern.clone())
            .take(5)
            .collect::<Vec<_>>()
            .join("?");

        let groups = (0..5).flat_map(|_| groups.iter()).cloned().collect();

        let result = Arc::clone(&result);
        let finished = Arc::clone(&finished);
        let handle = thread::spawn(move || {
            let full_regex_pattern = create_full_regex_pattern(&groups);
            let full_regex = Regex::new(&full_regex_pattern).expect("Invalid regex pattern");

            let mut pattern_stack = Vec::new();
            pattern_stack.push(pattern.clone());

            while let Some(pattern) = pattern_stack.pop() {
                // Find the first index of `?` in the pattern.
                if let Some(idx) = pattern.find('?') {
                    if !test_pattern(&pattern, &groups) {
                        // Move on folks
                        continue;
                    }

                    // Replace the first `?` with a `.`.
                    let mut dot_pattern = pattern.clone();
                    dot_pattern.replace_range(idx..idx + 1, ".");
                    pattern_stack.push(dot_pattern.clone());

                    // Replace the first `?` with a `#`.
                    let mut hash_pattern = pattern.clone();
                    hash_pattern.replace_range(idx..idx + 1, "#");
                    pattern_stack.push(hash_pattern.clone());
                } else if full_regex.is_match(&pattern) {
                    // We didn't find a ? which means we should do a full test
                    let mut result = result.lock().unwrap();
                    *result += 1;
                }
            }

            let mut finished = finished.lock().unwrap();
            *finished += 1;
            info!("Finished {} out of {}", finished, total);
            info!("\tOrigPattern\t{}", orig_pattern.clone());
            info!("\tCurrResult\t{}", result.lock().unwrap());
        });

        handles.push(handle);
    }

    let mut finished = 0;
    for handle in handles {
        handle.join().unwrap();
    }

    let result = result.lock().unwrap().clone();
    Ok(result)
}

fn test_pattern(pattern: &String, groups: &Vec<u8>) -> bool {
    let mut end_length_check = false;
    let (mut pattern_to_test, _) = pattern.split_once('?').unwrap();
    if pattern_to_test.ends_with('#') {
        end_length_check = true;
    }

    let things: Vec<&str> = pattern_to_test
        .split('.')
        .filter(|s| !s.is_empty())
        .collect();

    if things.len() == 0 {
        return true;
    }

    if things.len() > groups.len() {
        return false;
    }

    for (idx, thing) in things.iter().enumerate() {
        let group = groups.get(idx).unwrap();
        if idx == things.len() - 1 && end_length_check {
            // We are at the end and we just need to check the length
            if thing.len() > *group as usize {
                return false;
            }
        } else {
            if thing.len() != *group as usize {
                return false;
            }
        }
    }

    return true;
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        let input = include_str!("../test_input.txt");
        assert_eq!(525152, process(input)?);
        Ok(())
    }
}
