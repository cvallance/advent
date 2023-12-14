// You finally reach the hot springs! You can see steam rising from secluded areas attached to the primary, ornate building.
//
// As you turn to enter, the researcher stops you. "Wait - I thought you were looking for the hot springs, weren't you?" You indicate that this definitely looks like hot springs to you.
//
// "Oh, sorry, common mistake! This is actually the onsen! The hot springs are next door."
//
// You look in the direction the researcher is pointing and suddenly notice the massive metal helixes towering overhead. "This way!"
//
// It only takes you a few more steps to reach the main gate of the massive fenced-off area containing the springs. You go through the gate and into a small administrative building.
//
// "Hello! What brings you to the hot springs today? Sorry they're not very hot right now; we're having a lava shortage at the moment." You ask about the missing machine parts for Desert Island.
//
// "Oh, all of Gear Island is currently offline! Nothing is being manufactured at the moment, not until we get more lava to heat our forges. And our springs. The springs aren't very springy unless they're hot!"
//
// "Say, could you go up and see why the lava stopped flowing? The springs are too cold for normal operation, but we should be able to find one springy enough to launch you up there!"
//
// There's just one problem - many of the springs have fallen into disrepair, so they're not actually sure which springs would even be safe to use! Worse yet, their condition records of which springs are damaged (your puzzle input) are also damaged! You'll need to help them repair the damaged records.
//
// In the giant field just outside, the springs are arranged into rows. For each row, the condition records show every spring and whether it is operational (.) or damaged (#). This is the part of the condition records that is itself damaged; for some springs, it is simply unknown (?) whether the spring is operational or damaged.
//
// However, the engineer that produced the condition records also duplicated some of this information in a different format! After the list of springs for a given row, the size of each contiguous group of damaged springs is listed in the order those groups appear in the row. This list always accounts for every damaged spring, and each number is the entire size of its contiguous group (that is, groups are always separated by at least one operational spring: #### would always be 4, never 2,2).
//
// So, condition records with no unknown spring conditions might look like this:
//
// #.#.### 1,1,3
// .#...#....###. 1,1,3
// .#.###.#.###### 1,3,1,6
// ####.#...#... 4,1,1
// #....######..#####. 1,6,5
// .###.##....# 3,2,1
// However, the condition records are partially damaged; some of the springs' conditions are actually unknown (?). For example:
//
// ???.### 1,1,3
// .??..??...?##. 1,1,3
// ?#?#?#?#?#?#?#? 1,3,1,6
// ????.#...#... 4,1,1
// ????.######..#####. 1,6,5
// ?###???????? 3,2,1
// Equipped with this information, it is your job to figure out how many different arrangements of operational and broken springs fit the given criteria in each row.
//
// In the first line (???.### 1,1,3), there is exactly one way separate groups of one, one, and three broken springs (in that order) can appear in that row: the first three unknown springs must be broken, then operational, then broken (#.#), making the whole row #.#.###.
//
// The second line is more interesting: .??..??...?##. 1,1,3 could be a total of four different arrangements. The last ? must always be broken (to satisfy the final contiguous group of three broken springs), and each ?? must hide exactly one of the two broken springs. (Neither ?? could be both broken springs or they would form a single contiguous group of two; if that were true, the numbers afterward would have been 2,3 instead.) Since each ?? can either be #. or .#, there are four possible arrangements of springs.
//
// The last line is actually consistent with ten different arrangements! Because the first number is 3, the first and second ? must both be . (if either were #, the first number would have to be 4 or higher). However, the remaining run of unknown spring conditions have many different ways they could hold groups of two and one broken springs:
//
// ?###???????? 3,2,1
// .###.##.#...
// .###.##..#..
// .###.##...#.
// .###.##....#
// .###..##.#..
// .###..##..#.
// .###..##...#
// .###...##.#.
// .###...##..#
// .###....##.#
// In this example, the number of possible arrangements for each row is:
//
// ???.### 1,1,3 - 1 arrangement
// .??..??...?##. 1,1,3 - 4 arrangements
// ?#?#?#?#?#?#?#? 1,3,1,6 - 1 arrangement
// ????.#...#... 4,1,1 - 1 arrangement
// ????.######..#####. 1,6,5 - 4 arrangements
// ?###???????? 3,2,1 - 10 arrangements
// Adding all of the possible arrangement counts together produces a total of 21 arrangements.
//
// For each row, count all of the different arrangements of operational and broken springs that meet the given criteria. What is the sum of those counts?

use crate::custom_error::AocError;
use crate::shared::{
    create_full_regex_pattern, create_regex_pattern, find_char_indexes, parse_input,
};
use regex::Regex;
use std::collections::HashMap;
use std::sync::{Arc, Mutex};
use std::thread;
use tracing::info;

pub fn process(input: &str) -> miette::Result<u64, AocError> {
    // let input = include_str!("../test_input.txt");
    // let input = include_str!("../test_input_two.txt");

    let result: Arc<Mutex<u64>> = Arc::new(Mutex::new(0));
    let mut handles = vec![];

    let items = parse_input(input);
    for (mut pattern, groups) in items {
        let result = Arc::clone(&result);
        let handle = thread::spawn(move || {
            info!("Pattern: {}", pattern);
            info!("Groups: {:?}", groups);
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
                    let mut pattern = pattern.clone();
                    pattern.replace_range(idx..idx + 1, ".");
                    if test_pattern(&pattern, &groups, &regex_map) {
                        // info!("\tYES");
                        pattern_stack.push(pattern.clone());
                    }

                    // Replace the first `?` with a `#`.
                    let mut pattern = pattern.clone();
                    pattern.replace_range(idx..idx + 1, "#");
                    if test_pattern(&pattern, &groups, &regex_map) {
                        // info!("\tYES");
                        pattern_stack.push(pattern.clone());
                    }
                } else if full_regex.is_match(&pattern) {
                    // We didn't find a ? which means we should do a full test
                    let mut result = result.lock().unwrap();
                    *result += 1;
                }
            }
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
        assert_eq!(21, process(input)?);
        Ok(())
    }
}
