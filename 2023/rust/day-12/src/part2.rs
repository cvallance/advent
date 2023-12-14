use crate::custom_error::AocError;
use crate::shared::{create_full_regex_pattern, create_regex_pattern, parse_input};
use regex::Regex;
use std::collections::HashMap;
use std::sync::{Arc, Mutex};
use std::thread;
use tracing::info;

pub fn process(input: &str) -> miette::Result<u64, AocError> {
    let input = include_str!("../test_input.txt");
    // let input = include_str!("../test_input_two.txt");

    let result: Arc<Mutex<u64>> = Arc::new(Mutex::new(0));
    let mut handles = vec![];

    let items = parse_input(input);
    for (orig_pattern, groups) in items {
        // let orig_pattern = orig_pattern.clone();
        let pattern = std::iter::repeat(orig_pattern.clone())
            .take(5)
            .collect::<Vec<_>>()
            .join("?");

        let groups = (0..5).flat_map(|_| groups.iter()).cloned().collect();

        let result = Arc::clone(&result);
        let handle = thread::spawn(move || {
            let full_regex_pattern = create_full_regex_pattern(&groups);
            let full_regex = Regex::new(&full_regex_pattern).expect("Invalid regex pattern");

            let mut regex_map = HashMap::new();
            for i in 1..=groups.len() {
                let regex_pattern = create_regex_pattern(&groups, i);
                let regex = Regex::new(&regex_pattern).expect("Invalid regex pattern");
                regex_map.insert(i, regex);
            }

            let mut pattern_stack = Vec::new();
            pattern_stack.push(pattern.clone());

            while let Some(pattern) = pattern_stack.pop() {
                // Find the first index of `?` in the pattern.
                if let Some(idx) = pattern.find('?') {
                    // Replace the first `?` with a `.`.
                    let mut dot_pattern = pattern.clone();
                    dot_pattern.replace_range(idx..idx + 1, ".");
                    if test_pattern(&dot_pattern, &groups, &regex_map) {
                        // info!("\tYES");
                        pattern_stack.push(dot_pattern.clone());
                    }

                    // Replace the first `?` with a `#`.
                    let mut hash_pattern = pattern.clone();
                    hash_pattern.replace_range(idx..idx + 1, "#");
                    if test_pattern(&hash_pattern, &groups, &regex_map) {
                        // info!("\tYES");
                        pattern_stack.push(hash_pattern.clone());
                    }
                } else if full_regex.is_match(&pattern) {
                    // We didn't find a ? which means we should do a full test
                    let mut result = result.lock().unwrap();
                    *result += 1;
                }
            }

            info!("Finished");
            info!("\tOrigPattern\t{}", orig_pattern.clone());
            info!("\tCurrResult\t{}", result.lock().unwrap());
        });

        handles.push(handle);
    }

    for handle in handles {
        handle.join().unwrap();
    }

    let result = result.lock().unwrap().clone();
    Ok(result)
}

fn test_pattern(pattern: &String, groups: &Vec<u8>, regex_map: &HashMap<usize, Regex>) -> bool {
    let mut pattern_to_test = if let Some((start, _end)) = pattern.split_once('?') {
        start.trim_end_matches('#').to_string()
    } else {
        pattern.to_string()
    };

    let count = pattern_to_test.split('.').filter(|s| !s.is_empty()).count();

    if count == 0 {
        return true;
    }

    if let Some(regex) = regex_map.get(&count) {
        regex.is_match(&pattern_to_test)
    } else {
        false
    }
    // regex.is_match(&pattern_to_test)
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
