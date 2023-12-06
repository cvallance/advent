use nom::Map;
use std::collections::HashMap;
use tracing::info;

#[derive(Debug, Clone)]
pub struct Mapping {
    pub dest: String,
    pub source: String,
    pub ranges: Vec<Range>,
}

#[derive(Debug, Clone)]
pub struct Range {
    pub dest: u64,
    pub source: u64,
    pub length: u64,
}

pub fn parse_input(input: &str) -> (Vec<u64>, Vec<Mapping>) {
    let seeds_line = input.lines().next().unwrap();
    let seeds_line = seeds_line.trim_start_matches("seeds: ");
    let seed_values: Vec<u64> = seeds_line
        .split_whitespace()
        .map(|s| s.parse().unwrap())
        .collect();

    let mut mappings = Vec::new();
    let mut current_mapping = Mapping {
        source: "".to_string(),
        dest: "".to_string(),
        ranges: vec![],
    };
    for line in input.lines().skip(2) {
        if line.is_empty() {
            mappings.push(current_mapping.clone());
            continue;
        } else if line.contains("-to-") {
            let (source, dest) = line.split_once("-to-").unwrap();
            current_mapping = Mapping {
                source: source.to_string(),
                dest: dest.trim_end_matches(" map:").to_string(),
                ranges: Vec::new(),
            };
        } else {
            let parts: Vec<u64> = line
                .split_whitespace()
                .map(|x| x.parse().unwrap())
                .collect();
            current_mapping.ranges.push(Range {
                dest: parts[0],
                source: parts[1],
                length: parts[2],
            })
        }
    }

    mappings.push(current_mapping);

    (seed_values, mappings)
}

pub fn get_location(seed: u64, mappings: &Vec<Mapping>) -> u64 {
    let mut value = seed;
    for mapping in mappings {
        for range in &mapping.ranges {
            if range.source <= value && range.source + range.length > value {
                value = range.dest + (value - range.source);
                break;
            }
        }
    }

    value
}
