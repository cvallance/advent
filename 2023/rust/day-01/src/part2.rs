use std::collections::HashMap;
use crate::custom_error::AocError;

pub fn process(
    input: &str,
) -> miette::Result<i32, AocError> {
    let words = vec![
        "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
    ];
    let mut words_to_numbers: HashMap<&str, i32> = HashMap::new();
    words_to_numbers.insert("zero", 0);
    words_to_numbers.insert("one", 1);
    words_to_numbers.insert("two", 2);
    words_to_numbers.insert("three", 3);
    words_to_numbers.insert("four", 4);
    words_to_numbers.insert("five", 5);
    words_to_numbers.insert("six", 6);
    words_to_numbers.insert("seven", 7);
    words_to_numbers.insert("eight", 8);
    words_to_numbers.insert("nine", 9);
    words_to_numbers.insert("0", 0);
    words_to_numbers.insert("1", 1);
    words_to_numbers.insert("2", 2);
    words_to_numbers.insert("3", 3);
    words_to_numbers.insert("4", 4);
    words_to_numbers.insert("5", 5);
    words_to_numbers.insert("6", 6);
    words_to_numbers.insert("7", 7);
    words_to_numbers.insert("8", 8);
    words_to_numbers.insert("9", 9);

    let mut numbers = vec![];
    for line in input.lines() {
        let mut first_idx: i32 = -1;
        let mut first_word = "";
        let mut last_idx: i32 = -1;
        let mut last_word = "";

        for word in &words {
            if let Some(idx) = line.find(word) {
                let idx = idx as i32;
                if first_idx == -1 || idx < first_idx {
                    first_idx = idx;
                    first_word = word;
                }
            }

            if let Some(idx) = line.rfind(word) {
                let idx = idx as i32;
                if idx > last_idx {
                    last_idx = idx;
                    last_word = word;
                }
            }
        }

        numbers.push(words_to_numbers[first_word] * 10 + words_to_numbers[last_word]);
    }
    Ok(numbers.iter().sum())
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        let input = include_str!("../test_input2.txt");

        assert_eq!(281, process(input)?);
        Ok(())
    }
}