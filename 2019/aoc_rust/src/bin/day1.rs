use std::fs::File;
use std::io::{self, BufRead};
use std::path::Path;

fn main() {
    let lines = read_lines("day1_input.txt").unwrap();
    let mut part1_result: u32 = 0;
    let mut part2_result: u32 = 0;
    for line in lines {
        if let Ok(module_size) = line {
            let mut module_size: u32 = module_size.parse().unwrap();

            let fuel = calc_fuel(module_size);
            part1_result += fuel;
            part2_result += fuel;
            module_size = fuel;
            while module_size > 8 {
                let fuel = calc_fuel(module_size);
                part2_result += fuel;
                module_size = fuel;
            }
        }
    }

    println!("part 1 {}", part1_result);
    println!("part 2 {}", part2_result);
}

fn calc_fuel(module_size: u32) -> u32 {
    module_size / 3 - 2
}

fn read_lines<P>(filename: P) -> io::Result<io::Lines<io::BufReader<File>>>
where
    P: AsRef<Path>,
{
    let file = File::open(filename)?;
    Ok(io::BufReader::new(file).lines())
}
