use std::fs::File;
use std::io;
use std::io::BufRead;
use std::path::Path;

fn main() {
    let lines = read_lines("../../inputs/day1.txt").unwrap();
    let lines: Vec<String> = lines.collect::<Result<_, _>>().unwrap();

    // First
    let mut last: u32 = u32::MAX;
    let mut higher_count: u32 = 0;
    for line in &lines {
        let value: u32 = line.parse().unwrap();
        if value > last {
            higher_count += 1;
        }
        last = value;
    }

    println!("First {}", higher_count); // 1477

    // Second
    let mut last: u32 = u32::MAX;
    let mut higher_count: u32 = 0;

    let max = lines.len() - 3;
    for i in 0..=max {
        let one: u32 = lines[i].parse().unwrap();
        let two: u32 = lines[i+1].parse().unwrap();
        let three: u32 = lines[i+2].parse().unwrap();
        let value = one + two + three;
        if value > last {
            higher_count += 1;
        }
        last = value;
    }

    println!("Second {}", higher_count); // 1523
}

fn read_lines<P>(filename: P) -> io::Result<io::Lines<io::BufReader<File>>>
    where P: AsRef<Path>, {
    let file = File::open(filename)?;
    Ok(io::BufReader::new(file).lines())
}