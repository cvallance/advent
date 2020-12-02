use std::fs::File;
use std::io::prelude::*;
use std::io::BufReader;

const COLUMNS: usize = 25;
const ROWS: usize = 6;
const LAYER_TOTAL: usize = COLUMNS * ROWS;

#[derive(Debug)]
struct Layer {
    digit_counts: [u8; 3],
    rows: [[u8; COLUMNS]; ROWS],
}

fn main() {
    let file = File::open("day8_input.txt").unwrap();
    let mut reader = BufReader::new(file);
    let mut buffer = [0; LAYER_TOTAL];

    let mut layers = Vec::new();
    let mut smallest_zeros = u8::MAX;
    let mut part1_result = 0;
    while let Ok(_) = reader.read(&mut buffer) {
        let mut current_layer = Layer {
            digit_counts: [0; 3],
            rows: [[0; COLUMNS]; ROWS],
        };

        for (i, char_byte) in buffer.iter().enumerate() {
            let column = i % COLUMNS;
            let row = i / COLUMNS;
            let value = (*char_byte as char).to_digit(10).unwrap() as u8;
            current_layer.digit_counts[value as usize] =
                current_layer.digit_counts[value as usize] + 1;
            current_layer.rows[row][column] = value;
        }

        let zeros_count = current_layer.digit_counts[0];
        if zeros_count < smallest_zeros {
            smallest_zeros = zeros_count;
            part1_result = current_layer.digit_counts[1] * current_layer.digit_counts[2];
        }

        println!("HERE 1");
        println!("{:?}", &current_layer);

        layers.push(current_layer);
    }

    let mut part2_result = 0;
    println!("part 1 {}", part1_result);
    println!("part 2 {}", part2_result);
}

fn calc_fuel(module_size: u32) -> u32 {
    module_size / 3 - 2
}
