// The engineer finds the missing part and installs it in the engine! As the engine springs to life, you jump in the closest gondola, finally ready to ascend to the water source.
//
// You don't seem to be going very fast, though. Maybe something is still wrong? Fortunately, the gondola has a phone labeled "help", so you pick it up and the engineer answers.
//
// Before you can explain the situation, she suggests that you look out the window. There stands the engineer, holding a phone in one hand and waving with the other. You're going so slowly that you haven't even left the station. You exit the gondola.
//
// The missing part wasn't the only issue - one of the gears in the engine is wrong. A gear is any * symbol that is adjacent to exactly two part numbers. Its gear ratio is the result of multiplying those two numbers together.
//
// This time, you need to find the gear ratio of every gear and add them all up so that the engineer can figure out which gear needs to be replaced.
//
// Consider the same engine schematic again:
//
// 467..114..
// ...*......
// ..35..633.
// ......#...
// 617*......
// .....+.58.
// ..592.....
// ......755.
// ...$.*....
// .664.598..
// In this schematic, there are two gears. The first is in the top left; it has part numbers 467 and 35, so its gear ratio is 16345. The second gear is in the lower right; its gear ratio is 451490. (The * adjacent to 617 is not a gear because it is only adjacent to one part number.) Adding up all of the gear ratios produces 467835.
//
// What is the sum of all of the gear ratios in your engine schematic?

use crate::custom_error::AocError;
use crate::shared::{parse_parts, Point};
use std::collections::{HashMap, HashSet};

pub fn process(input: &str) -> miette::Result<u32, AocError> {
    let schematic: Vec<Vec<char>> = input
        .lines()
        .map(|line| line.chars().collect())
        .collect::<Vec<_>>();

    let parts = parse_parts(&schematic);
    let mut found = HashMap::new();
    let mut result = 0;
    for part in parts {
        let adjacent = part.all_adjacent();
        for adj in adjacent {
            if adj.x < 0
                || adj.y < 0
                || adj.x >= schematic[0].len() as i32
                || adj.y >= schematic.len() as i32
            {
                continue;
            }

            if schematic[adj.y as usize][adj.x as usize] == '*' {
                let point = Point::new(adj.x, adj.y);
                if let Some(item) = found.remove(&point) {
                    result += item * part.number;
                } else {
                    found.insert(point, part.number);
                }
            }
        }
    }

    Ok(result)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        let input = include_str!("../test_input.txt");
        assert_eq!(467835, process(input)?);
        Ok(())
    }
}
