use std::collections::HashMap;

pub struct Node {
    pub left: String,
    pub right: String,
}

pub fn parse_input(input: &str) -> (Vec<char>, HashMap<String, Node>) {
    let instructions = input.lines().next().unwrap();
    let instructions: Vec<char> = instructions.chars().collect();

    let mut nodes = HashMap::new();
    for line in input.lines().skip(2) {
        let (source, left_and_right) = line.split_once(" = ").unwrap();
        let (left, right) = left_and_right
            .trim_matches(&['(', ')'][..])
            .split_once(", ")
            .unwrap();

        nodes.insert(
            source.to_string(),
            Node {
                left: left.to_string(),
                right: right.to_string(),
            },
        );
    }

    (instructions, nodes)
}
