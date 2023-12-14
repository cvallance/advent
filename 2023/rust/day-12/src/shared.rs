pub fn parse_input(input: &str) -> Vec<(String, Vec<u8>)> {
    input
        .lines()
        .map(|line| {
            let (pattern, numbers) = line.trim().split_once(' ').unwrap();
            let numbers = numbers
                .split(',') // Split the number string by commas.
                .map(|n| n.trim().parse::<u8>().expect("Expected a number")) // Parse each part into u8.
                .collect(); // Collect numbers into a Vec<u8>.
            (pattern.to_string(), numbers)
        })
        .collect()
}

pub fn find_char_indexes(s: &str, c: char) -> Vec<usize> {
    s.char_indices()
        .filter_map(|(index, char)| if char == c { Some(index) } else { None })
        .collect()
}

pub fn create_full_regex_pattern(numbers: &Vec<u8>) -> String {
    create_regex_pattern(numbers, numbers.len())
}

pub fn create_regex_pattern(numbers: &Vec<u8>, count: usize) -> String {
    let mut pattern = String::from(r"^\.*");

    for (i, num) in numbers.iter().enumerate() {
        if i == count {
            break;
        }

        pattern.push_str(&format!(r"#{{{}}}\.*", num));

        // Add a '.+' after each number except for the last one.
        if i != count - 1 {
            pattern.push_str(r"\.+");
        }
    }

    pattern.push_str(r"\.*$");

    pattern
}
