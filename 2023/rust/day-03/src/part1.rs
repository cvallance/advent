// You and the Elf eventually reach a gondola lift station; he says the gondola lift will take you up to the water source, but this is as far as he can bring you. You go inside.
//
// It doesn't take long to find the gondolas, but there seems to be a problem: they're not moving.
//
// "Aaah!"
//
// You turn around to see a slightly-greasy Elf with a wrench and a look of surprise. "Sorry, I wasn't expecting anyone! The gondola lift isn't working right now; it'll still be a while before I can fix it." You offer to help.
//
// The engineer explains that an engine part seems to be missing from the engine, but nobody can figure out which one. If you can add up all the part numbers in the engine schematic, it should be easy to work out which part is missing.
//
// The engine schematic (your puzzle input) consists of a visual representation of the engine. There are lots of numbers and symbols you don't really understand, but apparently any number adjacent to a symbol, even diagonally, is a "part number" and should be included in your sum. (Periods (.) do not count as a symbol.)
//
// Here is an example engine schematic:
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
// In this schematic, two numbers are not part numbers because they are not adjacent to a symbol: 114 (top right) and 58 (middle right). Every other number is adjacent to a symbol and so is a part number; their sum is 4361.
//
// Of course, the actual engine schematic is much larger. What is the sum of all of the part numbers in the engine schematic?

use crate::custom_error::AocError;
use crate::shared::parse_parts;
use std::collections::HashSet;

pub fn process(input: &str) -> miette::Result<u32, AocError> {
    let symbols = vec!['#', '*', '$', '+', '/', '@', '&', '!', '?', '%', '^', '='];
    let symbols: HashSet<char> = symbols.into_iter().collect();

    let schematic: Vec<Vec<char>> = input
        .lines()
        .map(|line| line.chars().collect())
        .collect::<Vec<_>>();

    let parts = parse_parts(&schematic);
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

            if symbols.contains(&schematic[adj.y as usize][adj.x as usize]) {
                result += part.number;
            }
        }
        println!("{:?}", part);
    }

    Ok(result)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_process() -> miette::Result<()> {
        let input = include_str!("../test_input.txt");
        assert_eq!(4361, process(input)?);
        Ok(())
    }
}
