use io::BufRead;
use std::{
    collections::{HashMap, HashSet},
    fs::File,
    io,
    path::Path,
};

fn main() {
    let lines = read_lines("day6_input.txt").unwrap();
    let lines: Vec<String> = lines.filter_map(Result::ok).collect();
    let orbiting_map = create_map(lines);
    let part1_distance = calculate_distances(&orbiting_map, "COM", 1);
    println!("part 1 {}", part1_distance);

    let part2_distance = calculate_distance_between_two(&orbiting_map);
    // println!("part 2 {:?}", to_you);

    println!("part 2 {}", part2_distance);
}

fn calculate_distance_between_two(orbiting_map: &HashMap<String, HashSet<String>>) -> usize {
    let to_you = path_to(&orbiting_map, "YOU", "COM", &mut Vec::new()).unwrap();
    let to_san = path_to(&orbiting_map, "SAN", "COM", &mut Vec::new()).unwrap();
    let mut index: usize = 0;
    loop {
        if to_you[index] != to_san[index] {
            break;
        }
        index += 1;
    }
    let result = to_you.len() + to_san.len() - index * 2;

    result
}

fn create_map(lines: Vec<String>) -> HashMap<String, HashSet<String>> {
    let mut orbiting_map: HashMap<String, HashSet<String>> = HashMap::new();

    for items in lines {
        let planets: Vec<&str> = items.split(')').collect();
        let planet1 = planets[0];
        let planet2 = planets[1];

        if let Some(orbiting_list) = orbiting_map.get_mut(planet1) {
            orbiting_list.insert(String::from(planet2));
        } else {
            let mut new_orbiting_list = HashSet::new();
            new_orbiting_list.insert(String::from(planet2));
            orbiting_map.insert(String::from(planet1), new_orbiting_list);
        }
    }
    orbiting_map
}

fn calculate_distances(
    orbiting_map: &HashMap<String, HashSet<String>>,
    planet_name: &str,
    current_level: u32,
) -> u32 {
    let mut total: u32 = 0;
    if let Some(orbiting_planets) = orbiting_map.get(planet_name) {
        total += orbiting_planets.len() as u32 * current_level;
        for orbiting_planet in orbiting_planets {
            total += calculate_distances(orbiting_map, orbiting_planet, current_level + 1);
        }
    }
    return total;
}

fn path_to(
    orbiting_map: &HashMap<String, HashSet<String>>,
    planet_name: &str,
    current_planet: &str,
    current_path: &mut Vec<String>,
) -> Option<Vec<String>> {
    // Is there anything orbiting the current planet?
    if let Some(orbiting_planets) = orbiting_map.get(current_planet) {
        current_path.push(String::from(current_planet));
        // If there is loop them to try and find the planet, and if we don't find it, continue the search
        for orbiting_planet in orbiting_planets {
            if orbiting_planet == planet_name {
                return Some(current_path.clone());
            }

            if let Some(matched_path) = path_to(
                orbiting_map,
                planet_name,
                orbiting_planet,
                &mut current_path.clone(),
            ) {
                return Some(matched_path);
            }
        }
    }

    None
}

fn read_lines<P>(filename: P) -> io::Result<io::Lines<io::BufReader<File>>>
where
    P: AsRef<Path>,
{
    let file = File::open(filename)?;
    Ok(io::BufReader::new(file).lines())
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_1() {
        let map = test_1_map();
        let result = calculate_distances(&map, "COM", 1);

        assert_eq!(result, 42);
    }

    #[test]
    fn test_2() {
        let map = test_2_map();
        let result = calculate_distance_between_two(&map);

        assert_eq!(result, 4);
    }

    fn test_1_map() -> HashMap<String, HashSet<String>> {
        let lines = vec![
            String::from("COM)B"),
            String::from("B)C"),
            String::from("C)D"),
            String::from("D)E"),
            String::from("E)F"),
            String::from("B)G"),
            String::from("G)H"),
            String::from("D)I"),
            String::from("E)J"),
            String::from("J)K"),
            String::from("K)L"),
        ];

        create_map(lines)
    }

    fn test_2_map() -> HashMap<String, HashSet<String>> {
        let lines = vec![
            String::from("COM)B"),
            String::from("B)C"),
            String::from("C)D"),
            String::from("D)E"),
            String::from("E)F"),
            String::from("B)G"),
            String::from("G)H"),
            String::from("D)I"),
            String::from("E)J"),
            String::from("J)K"),
            String::from("K)L"),
            String::from("K)YOU"),
            String::from("I)SAN"),
        ];

        create_map(lines)
    }
}
